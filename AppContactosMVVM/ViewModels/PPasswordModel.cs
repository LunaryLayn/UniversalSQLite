using AppContactosMVVM.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace AppContactosMVVM.ViewModels
{
    public class PPasswordModel : ViewModelBase
    {
        public static ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        string p;

        public ICommand ComandoAceptar { get; set; }
        public override Task OnNavigatedFrom(NavigationEventArgs args)
        {
            return null;
        }

        public override Task OnNavigatedTo(NavigationEventArgs args)
        {
            return null;
        }

        public PPasswordModel()
        {
            ComandoAceptar = new Command(AccionAceptar);
        }

        private async void AccionAceptar(object obj)
        {
            if (!localSettings.Values.ContainsKey("Password"))
            {
                localSettings.Values["Password"] = Password;
                p = "valido";
            }

            else
            {
                if (localSettings.Values["Password"].ToString() == Password)
                {
                    p = "valido";
                }
                else
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("Contraseña no valida");

                    await messageDialog.ShowAsync();
                }
            }

            if (p == "valido")
            {
                Password = "";
                AppFrame.Navigate(typeof(MainPage));
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value;  RaisePropertyChanged("Password"); }
        }
    }
}
