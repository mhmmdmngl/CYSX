using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace CYS.Repos
{
	public class olcumCTX
	{
		public List<olcum> olcumList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<olcum>(sorgu, param).ToList();
				olcumSessionCTX osCTX = new olcumSessionCTX();
				foreach(var item in list)
				{
					item.olcumSession = osCTX.olcumTek("select * from olcumsession where id = @id", new { id = item.olcumSessionId });
				}
				
				return list;
			}
		}

		public olcum olcumTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<olcum>(sorgu, param).FirstOrDefault();
				olcumSessionCTX osCTX = new olcumSessionCTX();

				if (item != null)
				{
					item.olcumSession = osCTX.olcumTek("select * from olcumsession where id = @id", new { id = item.olcumSessionId });
				}
				return item;
			}
		}

		public int insert(olcum ol)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into olcum (olcumSessionId, adet, sonGuncelleme) values (@olcumSessionId, @adet, @sonGuncelleme)", ol);
				return item;
			}
		}

		public int update(olcum hayvan)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update  olcum set olcumSessionId = @olcumSessionId,adet=@adet, sonGuncelleme = @sonGuncelleme where id = @id", hayvan);
				return item;
			}
		}
	}
}
