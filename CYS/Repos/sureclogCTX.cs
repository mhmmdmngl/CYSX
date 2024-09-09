using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class sureclogCTX
	{
		public List<sureclog> sureclogList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<sureclog>(sorgu, param).ToList();
				surecCTX uctx = new surecCTX();
				foreach (var item in list)
				{
					item.process = uctx.surecTek("select * from surec where id = @id", new { id = item.processId });
				}
				return list;
			}

		}

		public sureclog sureclogTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<sureclog>(sorgu, param).FirstOrDefault();
				surecCTX uctx = new surecCTX();
				if (item != null)
					item.process = uctx.surecTek("select * from surec where id = @id", new { id = item.processId });

				return item;
			}
		}

		public int sureclogEkle(sureclog kategori)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				try {
					var item = connection.Execute("insert into sureclog (processId, fonksiyonAdi, sorguSonucu, sorguCevap) values (@processId, @fonksiyonAdi, @sorguSonucu, @sorguCevap)", kategori);
					return item;
				}
				catch(Exception e)
				{
					return 0;
				}
				
			}
		}

		
	}
}
