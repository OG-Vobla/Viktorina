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
		int cnt1;
		public GamePage()
		{
			InitializeComponent();
			questionList = Question.FindAllInDb();
			rnd = new Random();
			cnt1 = 0;
			labels = new List<System.Windows.Controls.Label>();
			answerLabels = new List<System.Windows.Controls.Label>();
			Buttons = new List<Button>();
			newMargin = 0;
			Buttons.Clear();
			TakeRandomQuest();
			SetAnswer();
			closeNums = new int[question.Answer.Length];
			Answer = new char[question.Answer.Length];
			newMargin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
			cnt = 0;
			SetKeyboard();
		}
		public void SetAllParam()
		{
			cnt1 = 0;

			newMargin = 0;
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
			labels = new List<System.Windows.Controls.Label>();
			answerLabels = new List<System.Windows.Controls.Label>();
			Buttons = new List<Button>();
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
			if (questionList.Count == 0)
			{
				MessageBox.Show("Вопросы закончились");
				this.NavigationService.GoBack();
				return;
			}
			question = questionList[random];
			Description.Text = question.Description;
		}
		public void SetAnswer()
		{
			int margin;
			int margin1 = 0 ;
			if (question.Answer.Length > 20)
			{
				margin = Convert.ToInt32(canvasmiddle.Width / 2 - ((question.Answer.Length - (question.Answer.Length - 20)) * 36) / 2);
				margin1 = Convert.ToInt32(canvasmiddle.Width / 2 - ((question.Answer.Length - (question.Answer.Length - 20)) * 36) / 2);

			}
			else
			{
				 margin = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);

				margin1 = Convert.ToInt32(canvasmiddle.Width / 2 - (question.Answer.Length * 36) / 2);
			}
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
				if (cnt1 >= 20)
				{
					if (cnt1 == 20)
					{
						m.Left = margin1;
						margin = margin1 + 36;
					}
					m.Top = 37;
				}
				cnt1++;
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
			if(cnt < question.Answer.Length)
			{
				canvas.Children.Remove(button);
				Answer[cnt] = Convert.ToChar(button.Content);
				System.Windows.Controls.Label Buttonitem = new System.Windows.Controls.Label();
				Buttonitem.Content = button.Content;
				Buttonitem.Height = 35;
				Buttonitem.Width = 35;
				Buttonitem.Background = new SolidColorBrush(Colors.White);
				Thickness m = Buttonitem.Margin;
				m.Left = newMargin;
				newMargin += 36;
				Buttonitem.Margin = labels[cnt].Margin;
				Buttonitem.Foreground = new SolidColorBrush(Colors.Black);
				Buttonitem.FontFamily = new FontFamily("Comic Sans MS");
				Buttonitem.FontSize = 20;
				answerLabels.Add(Buttonitem);
				canvasmiddle.Children.Remove(labels[cnt]);
				canvasmiddle.Children.Add(Buttonitem);
				cnt++;
			}
		}
		public void SetKeyboard()
		{
			int margin = 0;
			int cnt = 0;
			List<int> rndNums = new List<int>();
			
			for(int i = 0; i < question.Answer.Length; i++)
			{
				rndNums.Add(99);	
			}
			for (int i = 0; i < rndNums.Count; i++)
			{
				rndNums[i] = rnd.Next(1, 41);
				while(rndNums.Any(s => s == rndNums[i] && rndNums.IndexOf(s) != i))
				{
					rndNums[i] = rnd.Next(1, 41);
				}
					
				
			}
			for (int i = 1; i <= 40; i++)
			{
				Button Buttonitem = new Button();
				Buttonitem.Content = (char)(rnd.Next(1040, 1071));
				for (int j = 0; j < rndNums.Count; j++)
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
				if (i > 20)
				{
					if (i == 21)
					{
						m.Left = 0;
						margin = 36;
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
					/*					foreach (var j in Buttons)
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
					*/
					SetAllParam();
					return;
				}
			}
			MessageBox.Show("Правильно");
			questionList.Remove(question);
			/*if(questionList.Count == 0)
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
			SetKeyboard();*/
			SetAllParam();
			questionList.Remove(question);
		}
	}
	
}

