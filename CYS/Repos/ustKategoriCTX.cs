using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
    public class ustKategoriCTX
    {
        public List<Ustkategori> ustKategoriList(string sorgu, object param)
        {
            using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
            {
                var list = connection.Query<Ustkategori>(sorgu, param).ToList();
               
                return list;
            }
        }

        public Ustkategori ustKategoriTek(string sorgu, object param)
        {
            using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
            {
                var item = connection.Query<Ustkategori>(sorgu, param).FirstOrDefault();
               
                return item;
            }
        }

        public int ustKategoriEkle(Ustkategori ustkategori)
        {
            using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
            {
                var item = connection.Execute("insert into  ustkategori (name) values (@name)", ustkategori);
                return item;
            }
        }

        public int ustKategoriGuncelle(Ustkategori ustkategori)
        {
            using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
            {
                var item = connection.Execute("update  ustkategori set name = @name where id = @id", ustkategori);
                return item;
            }
        }
    }
}
