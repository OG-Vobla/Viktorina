using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Viktorina
{
	/// <summary>
	/// Логика взаимодействия для CreateTaskPage.xaml
	/// </summary>
	public partial class CreateTaskPage : Page
	{
		public CreateTaskPage()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			if(Description.Text != "" && Answer.Text != "")
			{
				foreach	(char i in Answer.Text)
				{
					if((int)i < ((1040)) || (int)i > ((1071)))
					{
						MessageBox.Show("Ответ должен состоять из больших букв русского алфавита.");
						return;
					}
				}
				new Question(Description.Text, Answer.Text).AddToDb();
				Description.Text = "";
				Answer.Text = "";
			}
			else
			{
				MessageBox.Show("Не все поля заполнены.");
			}

		}
	}
}
