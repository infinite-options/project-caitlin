using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCalendar
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {

            InitializeComponent();

            PrintConsole.Clicked += delegate
            {

                var eventName = name.Text;
                Console.WriteLine("eventName: " + eventName);

                var startTime = start.Text;
                Console.WriteLine("startTime: " + startTime);

                var endTime = end.Text;
                Console.WriteLine("endTime: " + endTime);


            };
   
        }
    }
}
