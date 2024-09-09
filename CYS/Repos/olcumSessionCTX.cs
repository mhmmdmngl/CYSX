using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class olcumSessionCTX
	{
		public List<olcumSession> olcumSessionList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<olcumSession>(sorgu, param).ToList();

				return list;
			}
		}

		public olcumSession olcumTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<olcumSession>(sorgu, param).FirstOrDefault();

				return item;
			}
		}

		public int insert(olcumSession ol)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into olcumsession (sessionGuid, tarih) values (@sessionGuid, @tarih)", ol);
				return item;
			}
		}

		public int update(olcumSession ol)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update  olcumsession set sessionGuid = @sessionGuid,tarih=@tarih where id = @id", ol);
				return item;
			}
		}
	}
}
