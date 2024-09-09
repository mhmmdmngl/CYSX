using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class processCTX
	{
		public List<process> processList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<process>(sorgu, param).ToList();

				return list;
			}
		}

		public process processTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<process>(sorgu, param).FirstOrDefault();

				return item;
			}
		}

		public int processEkle(process kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into process (processTypeId, requestId, baslangicZamani, bitisZamani, tamamlandi) values (@processTypeId, @requestId, @baslangicZamani, @bitisZamani, @tamamlandi)", kriter);
				return item;
			}
		}

		public int processtypeGuncelle(processtype kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update processtype set processTypeId = @processTypeId, requestId= @processTypeId, baslangicZamani= @processTypeId, bitisZamani= @processTypeId, tamamlandi= @processTypeId where id = @id", kriter);
				return item;
			}
		}
	}
}
