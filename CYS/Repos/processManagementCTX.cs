using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

public class processmanagementCTX
{
	private readonly string _connectionString = "Server=185.106.20.137;Database=CYS;User Id=abulu;Password=Merlab.2642;";

	// Get All (cihazList formatında)
	public List<processmanagement> GetAll(string sorgu, object param)
	{
		using (var connection = new MySqlConnection(_connectionString))
		{
			var list = connection.Query<processmanagement>(sorgu, param).ToList();
			return list;
		}
	}

	// Get Single (cihazTek formatında)
	public processmanagement Get(string sorgu, object param)
	{
		using (var connection = new MySqlConnection(_connectionString))
		{
			var item = connection.Query<processmanagement>(sorgu, param).FirstOrDefault();
			return item;
		}
	}

	// Insert (cihazEkle formatında)
	public int Add(processmanagement entity)
	{
		using (var connection = new MySqlConnection(_connectionString))
		{
			var result = connection.Execute("INSERT INTO processmanagement (guid, hayvangirdi, ilkkapikapandi, kupeokundu, okunankupe, sonagirlikalindimi, sonagirlik, cikiskapisiacildimi, tarih, cikisbeklemeagirligi, minimumhassasiyetagirlik) " +
											"VALUES (@guid, @hayvangirdi, @ilkkapikapandi, @kupeokundu, @okunankupe, @sonagirlikalindimi, @sonagirlik, @cikiskapisiacildimi, @tarih, @cikisbeklemeagirligi, @minimumhassasiyetagirlik)", entity);
			return result;
		}
	}

	// Update (cihazGuncelle formatında)
	public int Update(processmanagement entity)
	{
		using (var connection = new MySqlConnection(_connectionString))
		{
			var result = connection.Execute("UPDATE processmanagement SET guid = @guid, hayvangirdi = @hayvangirdi, ilkkapikapandi = @ilkkapikapandi, kupeokundu = @kupeokundu, okunankupe = @okunankupe, sonagirlikalindimi = @sonagirlikalindimi, sonagirlik = @sonagirlik, cikiskapisiacildimi = @cikiskapisiacildimi, tarih = @tarih, cikisbeklemeagirligi = @cikisbeklemeagirligi, minimumhassasiyetagirlik = @minimumhassasiyetagirlik WHERE id = @id", entity);
			return result;
		}
	}

	// Delete
	public int Delete(int id)
	{
		using (var connection = new MySqlConnection(_connectionString))
		{
			var result = connection.Execute("DELETE FROM processmanagement WHERE id = @id", new { id });
			return result;
		}
	}
}
