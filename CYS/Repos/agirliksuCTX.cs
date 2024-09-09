﻿using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class agirliksuCTX
	{
		public List<agirliksu> agirliksuList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<agirliksu>(sorgu, param).ToList();
				HayvanCTX hctx = new HayvanCTX();
				UserCTX uctx = new UserCTX();
				foreach(var item in list)
				{
					item.hayvan = hctx.hayvanTek("select * from hayvan where id = @id", new { id = item.hayvanId }); 
					item.user = uctx.userTek("select * from user where id = @id", new {id = item.userId}); ;
				}
				return list;
			}
		}

		public agirliksu agirliksuTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<agirliksu>(sorgu, param).FirstOrDefault();
				HayvanCTX hctx = new HayvanCTX();
				UserCTX uctx = new UserCTX();
				if (item != null)
				{
					item.hayvan = hctx.hayvanTek("select * from hayvan where id = @id", new { id = item.hayvanId });
					item.user = uctx.userTek("select * from user where id = @id", new { id = item.userId }); 
				}
				return item;
			}
		}

		public int agirliksuEkle(agirliksu kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into agirliksu (userId, hayvanId, ilkOlcum, sonOlcum, tarih, reqestId, hayvangirdi, hayvancikti, hayvanui) values (@userId, @hayvanId, @ilkOlcum, @sonOlcum, @tarih, @reqestId, @hayvangirdi, @hayvancikti, @hayvanui)", kriter);
				return item;
			}
		}

		public int agirliksuGuncelle(agirliksu kriter)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update agirliksu set userId = @userId, hayvanId= @hayvanId, ilkOlcum= @ilkOlcum, sonOlcum= @sonOlcum, tarih= @tarih, reqestId= @reqestId, hayvangirdi=@hayvangirdi, hayvancikti = @hayvancikti, hayvanui = @hayvanui where id = @id", kriter);
				return item;
			}
		}
	}
}
