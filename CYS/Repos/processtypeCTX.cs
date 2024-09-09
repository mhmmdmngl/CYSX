using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class processtypeCTX
	{
		public List<processtype> processtypeList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<processtype>(sorgu, param).ToList();

				return list;
			}
		}

		public processtype processtypeTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<processtype>(sorgu, param).FirstOrDefault();

				return item;
			}
		}

		public int processtypeEkle(processtype kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into processtype ( processAdi, processKodu) values (@processAdi, @processKodu)", kriter);
				return item;
			}
		}

		public int processtypeGuncelle(processtype kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update processtype set processAdi = @processAdi,processKodu=@processKodu where id = @id", kriter);
				return item;
			}
		}
	}
}
