using AppContactosMVVM.Conexiones;
using AppContactosMVVM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace AppContactosMVVM.ViewModels
{
    public class MainPageModel : INotifyPropertyChanged
    {
        private Conexion conexionDeDatos;

        public ICommand ComandoModificar { get; set; }
        public ICommand ComandoActualizar { get; set; }
        public ICommand ComandoEliminar { get; set; }

        public ICommand ComandoGuardar { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public MainPageModel()
        {
            conexionDeDatos = new Conexion();

            CSiDBExiste();

            ListaContactos = conexionDeDatos.LeerContactos();

            ComandoModificar = new Command(AccionModificar);
            ComandoActualizar = new Command(AccionActualizar);
            ComandoEliminar = new Command(AccionEliminar);

            ComandoGuardar = new Command(AccionGuardar);

            ActivarControlT = false;
            ActivarControlB1 = true;
            ActivarControlB3 = true;

        }

        private async void CSiDBExiste()
        {
            bool dbExist = await conexionDeDatos.compruebaSiExisteBD("BDAgenda.db");

            if (!dbExist)
            {
                //await CreateDatabaseAsync();
                conexionDeDatos.CreateDatabase();

                conexionDeDatos.addPrimerosContactos();

            }

        }

        private List<Contacto> listaContactos;

        public List<Contacto> ListaContactos
        {
            get { return listaContactos; }
            set
            {
                listaContactos = value;
                OnPropertyChanged("ListaContactos");
            }
        }

        private int pivotSeleccionado;
        public int PivotSeleccionado
        {
            get { return pivotSeleccionado; }
            set
            {
                pivotSeleccionado = value;
                OnPropertyChanged("PivotSeleccionado");

                if (pivotSeleccionado == 1)

                {
                    ContactoSeleccionado = null;

                }


            }
        }

        private Contacto contactoSeleccionado;
        public Contacto ContactoSeleccionado
        {
            get { return contactoSeleccionado; }
            set
            {
                contactoSeleccionado = value;
                OnPropertyChanged("ContactoSeleccionado");

                if (contactoSeleccionado != null)

                {

                    Nombre = contactoSeleccionado.Nombre;
                    Direccion = contactoSeleccionado.Direccion;
                    Telefono = contactoSeleccionado.Telefono;

                }

                else
                {
                    Nombre = "";
                    Direccion = "";
                    Telefono = "";

                }




            }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                nombre = value;
                OnPropertyChanged("Nombre");
            }
        }


        private string direccion;
        public string Direccion
        {
            get { return direccion; }
            set
            {
                direccion = value;
                OnPropertyChanged("Direccion");
            }
        }

        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set
            {
                telefono = value;
                OnPropertyChanged("Telefono");
            }
        }

        private bool activarControlT;

        public bool ActivarControlT
        {
            get { return activarControlT; }
            set
            {
                activarControlT = value;
                OnPropertyChanged("ActivarControlT");
            }
        }


        private bool activarControlB1;

        public bool ActivarControlB1
        {
            get { return activarControlB1; }
            set
            {
                activarControlB1 = value;
                OnPropertyChanged("ActivarControlB1");
            }
        }

        private bool activarControlB2;

        public bool ActivarControlB2
        {
            get { return activarControlB2; }
            set
            {
                activarControlB2 = value;
                OnPropertyChanged("ActivarControlB2");
            }
        }

        private bool activarControlB3;
        public bool ActivarControlB3
        {
            get { return activarControlB3; }
            set
            {
                activarControlB3 = value;
                OnPropertyChanged("ActivarControlB3");
            }
        }

        private async void AccionModificar(object parametro)
        {

            if (ContactoSeleccionado != null)
            {
                ActivarControlT = true;

                ActivarControlB1 = false;
                ActivarControlB2 = true;
                ActivarControlB3 = false;


            }
            else
            {


                string msg = "Tienes que seleccionar un contacto de la lista para poder Modificarlo";

                MessageDialog dialog = new MessageDialog(msg);
                await dialog.ShowAsync();
            }


        }

        private async void AccionActualizar(object parametro)
        {

            try
            {


                conexionDeDatos.modificarContacto(ContactoSeleccionado.Id, Nombre, Direccion, Telefono);

                string msg = "Contacto Modificado";

                MessageDialog dialog = new MessageDialog(msg);
                await dialog.ShowAsync();

                ListaContactos = conexionDeDatos.LeerContactos();


            }
            catch (Exception ex)
            {

                string msg = ex.Message;

                MessageDialog dialog = new MessageDialog(msg);
                await dialog.ShowAsync();
            }

            finally
            {
                ActivarControlT = false;

                ActivarControlB1 = true;
                ActivarControlB2 = false;
                ActivarControlB3 = true;



            }



        }

        private async void AccionEliminar(object parametro)
        {
            if (ContactoSeleccionado != null)
            {

                var messageDialog = new Windows.UI.Popups.MessageDialog("Seguro quieres borrar el contacto?");

                // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers

                //  using Windows.UI.Popups; necesario para  UICommand, UICommandInvokedHandler,UICommandInvokedHandler
                messageDialog.Commands.Add(new UICommand(
                    "OK",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.Commands.Add(new UICommand(
                    "Cancel",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 1;

                // Show the message dialog
                await messageDialog.ShowAsync();







            }
            else
            {
                //MessageBox.Show("Tienes que seleccionar un contacto de la lista para poder borrarlo",
                //    "Información", MessageBoxButton.OK, MessageBoxImage.Information);

                string msg = "Tienes que seleccionar un contacto de la lista para poder borrarlo";

                MessageDialog dialog = new MessageDialog(msg);
                await dialog.ShowAsync();
            }

        }

        private async void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "OK")
            {

                try
                {


                    conexionDeDatos.borrarContacto(ContactoSeleccionado.Id);
                    string msg = "Contacto Borrado";

                    MessageDialog dialog = new MessageDialog(msg);
                    await dialog.ShowAsync();

                    ListaContactos = conexionDeDatos.LeerContactos();

                    // ListaContactos.Remove(ContactoSeleccionado);

                }
                catch (Exception ex)
                {


                    string msg = ex.Message;

                    MessageDialog dialog = new MessageDialog(msg);
                    await dialog.ShowAsync();

                }


            }

            ContactoSeleccionado = null;
        }

        private async void AccionGuardar(object parametro)
        {
            Contacto contacto = new Contacto();
            contacto.Nombre = Nombre;
            contacto.Telefono = Telefono;
            contacto.Direccion = Direccion;


            conexionDeDatos.addContacto(contacto);

            string msg = "Contacto guardado";

            MessageDialog dialog = new MessageDialog(msg);
            await dialog.ShowAsync();

            ListaContactos = conexionDeDatos.LeerContactos();
            //ListaContactos.Add(contacto);

            ContactoSeleccionado = null;

            PivotSeleccionado = 0;



        }

    }
}
