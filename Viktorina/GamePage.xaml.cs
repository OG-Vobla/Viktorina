using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;

namespace Viktorina
{
	/// <summary>
	/// Логика взаимодействия для GamePage.xaml
	/// </summary>
	public partial class GamePage : Page
	{
		List<Question> questionList;
		Random Rnd;
		Question question;
		char[] Answer;
		List<System.Windows.Controls.Label> labels;
		int[] closeNums;
		int newMargin;
		
		public GamePage()
		{
			InitializeComponent();
			questionList = Question.FindAllInDb();
			Rnd = new Random();
			labels = new List<System.Windows.Controls.Label>();
			newMargin = 0;
			TakeRandomQuest();
			SetAnswer();
			closeNums = new int[question.Answer.Length];
			Answer = new char[question.Answer.Length];
			SetKeyboard();
		}
		public void TakeRandomQuest()
		{
			int random = Rnd.Next(0, questionList.Count);
			question = questionList[random];
			Description.Text = question.Description;
			questionList.Remove(question);
		}
		public void SetAnswer()
		{
			newMargin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
			int margin = Convert.ToInt32(canvasmiddle.Width/2 - (question.Answer.Length * 36)/2);
			foreach (var i in question.Answer)
			{
				
				System.Windows.Controls.Label Buttonitem = new System.Windows.Controls.Label();
				Buttonitem.Content = "";
				Buttonitem.Height = 35;
				Buttonitem.Width = 35;
				Buttonitem.Background = new SolidColorBrush(Colors.White);
				Thickness m = Buttonitem.Margin;
				m.Left = margin;
				margin += 36;
				Buttonitem.Margin = m;
				Buttonitem.Foreground = new SolidColorBrush(Colors.White);
				Buttonitem.Content = i;
				Buttonitem.FontFamily = new FontFamily("Comic Sans MS");
				Buttonitem.FontSize = 20;
				labels.Add(Buttonitem);
				canvasmiddle.Children.Add(Buttonitem);

			}
		}
		public void CheckSymbol(Button button)
		{
			foreach (var i in question.Answer)
			{
				if (button.Content.ToString() == i.ToString())
				{
					canvas.Children.Remove(button);
				}
			}
			
			for (int i =0;i<question.Answer.Length; i++)
			{
					Answer[i] = question.Answer[i];
					System.Windows.Controls.Label Buttonitem = new System.Windows.Controls.Label();
					Buttonitem.Content = question.Answer[i].ToString();
					Buttonitem.Height = 35;
					Buttonitem.Width = 35;
					Buttonitem.Background = new SolidColorBrush(Colors.White);
					Thickness m = Buttonitem.Margin;
					m.Left = newMargin + 36;
					Buttonitem.Margin = m;
					Buttonitem.Foreground = new SolidColorBrush(Colors.Black);
					Buttonitem.FontFamily = new FontFamily("Comic Sans MS");
					Buttonitem.FontSize = 20;
					canvasmiddle.Children.Add(Buttonitem);
				}
			}

		}
		public void SetKeyboard()
		{
			int margin = 0;
			int cnt = 0;
			int[] rndNums = new int[question.Answer.Length];
			for(int i = 0; i< rndNums.Length; i++)
			{
				rndNums[i] = Rnd.Next(0, 39);
				for (int j = 0; j < rndNums.Length; j++)
				{
					while (rndNums[i] == rndNums[j] && i != j)
					{
						rndNums[i] = Rnd.Next(0, 39);
					}
				}
			}
			for(int i = 0; i <= 40; i++)
			{
				Button Buttonitem = new Button();
				Buttonitem.Content = (char)(Rnd.Next(1040, 1071));
				for (int j = 0; j < rndNums.Length; j++)
				{
					if (i == rndNums[j])
					{
						Buttonitem.Content = question.Answer[j];
					}
				}
				Buttonitem.Height = 35;
				Buttonitem.Width = 35;
				Buttonitem.Background = new SolidColorBrush(Colors.White);
				Thickness m = Buttonitem.Margin;
				m.Left = margin;
				margin += 36;
				if (i >= 20)
				{
					if(i == 20)
					{
						m.Left = 0;
						margin = 0;
					}
					m.Top = 37;
				}
				Buttonitem.Margin = m;
				Buttonitem.Foreground = new SolidColorBrush(Colors.Black);
				Buttonitem.FontFamily = new FontFamily("Comic Sans MS");
				Buttonitem.FontSize = 20;
				Buttonitem.Click += (s, e) => { CheckSymbol(Buttonitem); };
				canvas.Children.Add(Buttonitem);
			}


			
		}
	}
}
