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
		Random rnd;
		Question question;
		char[] Answer;
		List<System.Windows.Controls.Label> labels;
		List<System.Windows.Controls.Label> answerLabels;
		List<Button> Buttons;
		int[] closeNums;
		int newMargin;
		int cnt;
		public GamePage()
		{
			InitializeComponent();
			questionList = Question.FindAllInDb();
			rnd = new Random();
			labels = new List<System.Windows.Controls.Label>();
			answerLabels = new List<System.Windows.Controls.Label>();
			Buttons = new List<Button>();
			newMargin = 0;
			TakeRandomQuest();
			SetAnswer();
			closeNums = new int[question.Answer.Length];
			Answer = new char[question.Answer.Length];
			newMargin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
			cnt = 0;
			SetKeyboard();
		}
		public void TakeRandomQuest()
		{
			int random = rnd.Next(0, questionList.Count);
			question = questionList[random];
			Description.Text = question.Description;
			questionList.Remove(question);
		}
		public void SetAnswer()
		{
			int margin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
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
/*			foreach (var i in question.Answer)
			{
				if (button.Content.ToString() == i.ToString())
				{
					canvas.Children.Remove(button);
				}
			}*/
			if(cnt < question.Answer.Length)
			{
				canvas.Children.Remove(button);
				Answer[cnt] = Convert.ToChar(button.Content);
				System.Windows.Controls.Label Buttonitem = new System.Windows.Controls.Label();
				Buttonitem.Content = button.Content;
				cnt++;
				Buttonitem.Height = 35;
				Buttonitem.Width = 35;
				Buttonitem.Background = new SolidColorBrush(Colors.White);
				Thickness m = Buttonitem.Margin;
				m.Left = newMargin;
				newMargin += 36;
				Buttonitem.Margin = m;
				Buttonitem.Foreground = new SolidColorBrush(Colors.Black);
				Buttonitem.FontFamily = new FontFamily("Comic Sans MS");
				Buttonitem.FontSize = 20;
				answerLabels.Add(Buttonitem);
				canvasmiddle.Children.Add(Buttonitem);

			}

		}
		public void SetKeyboard()
		{
			int margin = 0;
			int cnt = 0;
			int[] rndNums = new int[question.Answer.Length];
			for (int i = 0; i < rndNums.Length; i++)
			{
				rndNums[i] = rnd.Next(0, 39);
				for (int j = 0; j < rndNums.Length; j++)
				{
					while (rndNums[i] == rndNums[j] && i != j)
					{
						rndNums[i] = rnd.Next(0, 39);
					}
				}
			}
			for (int i = 0; i <= 40; i++)
			{
				Button Buttonitem = new Button();
				Buttonitem.Content = (char)(rnd.Next(1040, 1071));
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
					if (i == 20)
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
				Buttons.Add(Buttonitem);
				canvas.Children.Add(Buttonitem);
			}



		}
		private void Button_Click(object sender, RoutedEventArgs e)
		{
			for(int i = 0; i < question.Answer.Length; i++)
			{
				if (question.Answer[i] == Answer[i])
				{
					continue;
				}
				else
				{
					MessageBox.Show("Неправильно");
					foreach (var j in Buttons)
					{
						canvas.Children.Remove(j);
						canvas.Children.Add(j);
					}
					foreach (var j in labels)
					{
						canvasmiddle.Children.Remove(j);
					}
					foreach (var j in answerLabels)
					{
						canvasmiddle.Children.Remove(j);
					}
					labels = new List<System.Windows.Controls.Label>();
					answerLabels = new List<System.Windows.Controls.Label>();
					SetAnswer();
					closeNums = new int[question.Answer.Length];
					Answer = new char[question.Answer.Length];
					newMargin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
					cnt = 0;

					return;
				}
			}
			MessageBox.Show("Правильно");
			questionList.Remove(question);
			if(questionList.Count == 0)
			{
				MessageBox.Show("Вопросы закончились");
				this.NavigationService.GoBack();
				return;
			}
			foreach (var j in labels)
			{
				canvasmiddle.Children.Remove(j);
			}
			labels.Clear();
			foreach (var j in answerLabels)
			{
				canvasmiddle.Children.Remove(j);
			}
			answerLabels.Clear();
			foreach (var j in Buttons)
			{
				canvas.Children.Remove(j);
			}
			Buttons.Clear();
			TakeRandomQuest();
			SetAnswer();
			closeNums = new int[question.Answer.Length];
			Answer = new char[question.Answer.Length];
			newMargin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
			cnt = 0;
			SetKeyboard();
		}
	}
	
}

