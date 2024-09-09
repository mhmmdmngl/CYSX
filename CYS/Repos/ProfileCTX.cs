using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

namespace CYS.Repos
{
	public class ProfileCTX
	{
		public List<Profile> profilList(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var list = connection.Query<Profile>(sorgu, param).ToList();
				return list;
			}
		}

		public Profile profilTek(string sorgu, object param)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Query<Profile> (sorgu, param).FirstOrDefault();
				return item;
			}
		}

		public int profilEkle(Profile profil)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("insert into profile (userId,companyName,companyDescription,companyId, address,phoneNumber,cellPhoneNumber,logo) values (@userId,@companyName,@companyDescription,@companyId, @address,@phoneNumber,@cellPhoneNumber,@logo)", profil);
				return item;
			}
		}

		public int profilGuncelle(Profile profil)
		{
			using (var connection = new MySqlConnection("Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;"))
			{
				var item = connection.Execute("update profile set userId = @userId,companyName = @companyName,companyDescription = @companyDescription,address = @address,phoneNumber = @phoneNumber,cellPhoneNumber = @cellPhoneNumber,logo = @logo, cihazLink = @cihazLink where id = @id", profil);
				return item;
			}
		}
	}
}
