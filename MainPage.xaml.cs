using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        int counter = 0;
        int counter2 = 0;

        public MainPage()
        {
            InitializeComponent();

            brianButton.Clicked += brianButton_Clicked;
            sarahButton.Clicked += sarahButton_Clicked;
        }

        private async void brianButton_Clicked(object sender, EventArgs e)
        {
            if (counter % 2 == 0)
            {
                await Brian.TranslateTo(0, 0, 2000, Easing.Linear);
                counter++;
            }
            else
            {
                await Brian.TranslateTo(0, 300, 2000, Easing.Linear);
                counter++;
            }
        }


        private async void sarahButton_Clicked(object sender, EventArgs e)
        {
            if (counter2 % 2 == 0)
            {
                await Sarah.TranslateTo(0, -100, 2000, Easing.Linear);
                counter2++;
            }
            else
            {
                await Sarah.TranslateTo(0, 300, 2000, Easing.Linear);
                counter2++;
            }
        }


    }
}
