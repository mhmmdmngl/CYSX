using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class KriterCTX
	{
		public List<Kriter> kriterList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<Kriter>(sorgu, param).ToList();
				
				return list;
			}
		}

		public Kriter kriterTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<Kriter>(sorgu, param).FirstOrDefault();
				
				return item;
			}
		}

		public int KriterEkle(Kriter kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into kriter (kriterAdi) values (@kriterAdi)", kriter);
				return item;
			}
		}

		public int kriterGuncelle(Kriter kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update kriter set kriterAdi = @kriterAdi,isActive = @isActive where id = @id", kriter);
				return item;
			}
		}
	}
}
