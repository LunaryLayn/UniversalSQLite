using AppContactosMVVM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppContactosMVVM.Conexiones
{
    public class Conexion
    {
        //public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;


        string path;
        //SQLite.Net.SQLiteConnection conn;

        SQLite.SQLiteConnection conn;

        public Conexion()
        {

        }



        public async Task<bool> compruebaSiExisteBD(string nombreBD)
        {
            bool bdExiste = true;

            try
            {
                StorageFile sf = await ApplicationData.Current.LocalFolder.GetFileAsync(nombreBD);
            }
            catch (Exception)
            {
                bdExiste = false;
            }

            return bdExiste;
        }




        //private async Task CreateDatabaseAsync()
        //{

        //    SQLiteAsyncConnection conn = new SQLiteAsyncConnection(() =>
        //    {
        //        return new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(
        //         Path.Combine(ApplicationData.Current.LocalFolder.Path, "BDAgenda.db"), false));
        //    });

        //    await conn.CreateTableAsync<Contacto>();
        //}

        public void CreateDatabase()
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "BDAgenda.db");

            //conn = new SQLite.Net.SQLiteConnection(new
            //SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn = new SQLite.SQLiteConnection(path);


            conn.CreateTable<Contacto>();
            conn.Close();


        }

        public void addPrimerosContactos()
        {
            //Creamos una lista de usuarios
            var listaContactos = new List<Contacto>()
            {
                new Contacto()
                {
                    Nombre = "Javier",
                    Telefono = "669044233",
                    Direccion = "Salvador Tierra, 6",
                },
                new Contacto()
                {
                    Nombre = "Nieves",
                    Telefono = "689044245",
                    Direccion = "Roque Hermita, 2",
                },
                new Contacto()
                {
                    Nombre = "Lucia",
                    Telefono = "689590267",
                    Direccion = "-Gavilanescillos, 3",
                }
            };

            //Añadimos los usuarios a la tabla

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "BDAgenda.db");

            //conn = new SQLite.Net.SQLiteConnection(new
            //SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn = new SQLite.SQLiteConnection(path);

            conn.InsertAll(listaContactos);

            conn.Close();
        }

        public List<Contacto> LeerContactos()
        {
            List<Contacto> ListaDeContactos =
               new List<Contacto>();

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "BDAgenda.db");

            //conn = new SQLite.Net.SQLiteConnection(new
            //SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn = new SQLite.SQLiteConnection(path);

            var query = conn.Table<Contacto>();
            // contactos = await query.ToListAsync();

            ListaDeContactos = query.ToList<Contacto>();

            return ListaDeContactos;

        }

        public void addContacto(Contacto NContacto)
        {


            //Añadimos  Contacto a la tabla

            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "BDAgenda.db");

            //conn = new SQLite.Net.SQLiteConnection(new
            //SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn = new SQLite.SQLiteConnection(path);

            //Contacto contacto = new Contacto();
            //contacto.Nombre = nombre;
            //contacto.Telefono = telefono;
            //contacto.Direccion = direccion;

            conn.Insert(NContacto);

            conn.Close();
        }




        public void borrarContacto(int Identificador)
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "BDAgenda.db");

            //conn = new SQLite.Net.SQLiteConnection(new
            //SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn = new SQLite.SQLiteConnection(path);

            var contacto = conn.Table<Contacto>().Where(x => x.Id == Identificador).FirstOrDefault();
            if (contacto != null)
            {
                conn.Delete(contacto);
            }

        }

        public void modificarContacto(int Identificador, string nombre, string direccion, string telefono)
        {
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "BDAgenda.db");

            //conn = new SQLite.Net.SQLiteConnection(new
            //SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            conn = new SQLite.SQLiteConnection(path);

            var contacto = conn.Table<Contacto>().Where(x => x.Id == Identificador).FirstOrDefault();
            if (contacto != null)
            {
                contacto.Nombre = nombre;
                contacto.Direccion = direccion;
                contacto.Telefono = telefono;

                conn.Update(contacto);
            }

        }

    }
}
