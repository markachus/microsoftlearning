using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Phoneword
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        private string _translatedNumber;

        public MainPage()
        {
            InitializeComponent();
            this.BtnTranslate.Clicked += OnTranslate;
            this.BtnCall.Clicked += OnCall;
        }

        private async void OnCall(object sender, EventArgs e)
        {
            if (await DisplayAlert("Dial number", 
                $"Would you like to call to {_translatedNumber}", 
                "Yes", 
                "No")) {

                try
                {
                    PhoneDialer.Open(_translatedNumber);
                }
                catch (ArgumentNullException)
                {
                    await DisplayAlert("Unable to dial", "Phone number was not valid.", "OK");
                }
                catch (FeatureNotSupportedException)
                {
                    await DisplayAlert("Unable to dial", "Phone dialing not supported.", "OK");
                }
                catch (Exception)
                {
                    // Other error has occurred.
                    await DisplayAlert("Unable to dial", "Phone dialing failed.", "OK");
                }

            }
        }

        private void OnTranslate(object sender, EventArgs e)
        {
            _translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);

            if (!string.IsNullOrEmpty(_translatedNumber))
            {
                // TODO:
                BtnCall.IsEnabled = true;
                BtnCall.Text = "Call " + _translatedNumber;
            }
            else
            {
                BtnCall.IsEnabled = false;
                BtnCall.Text = "Call ";
            }

        }
    }
}
