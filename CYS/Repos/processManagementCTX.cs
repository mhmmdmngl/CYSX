using CYS.Models;
using Dapper;
using MySql.Data.MySqlClient;

public class processmanagementCTX
{
	private readonly string _connectionString = "Server=localhost;Database=CYS;User Id=root;Password=Muhamm3d!1;";

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
			var result = connection.Execute("INSERT INTO processmanagement (guid, hayvangirdi, ilkkapikapandi, kupeokundu, okunankupe, sonagirlikalindimi, sonagirlik, cikiskapisiacildimi, tarih, cikisbeklemeagirligi, minimumhassasiyetagirlik, cihazId, isTamamlandi, mevcutMod, hayvanid) " +
                                            "VALUES (@guid, @hayvangirdi, @ilkkapikapandi, @kupeokundu, @okunankupe, @sonagirlikalindimi, @sonagirlik, @cikiskapisiacildimi, @tarih, @cikisbeklemeagirligi, @minimumhassasiyetagirlik, @cihazId, @isTamamlandi, @mevcutMod, @hayvanid)", entity);
			return result;
		}
	}

    // Update (cihazGuncelle formatında)
    public int Update(processmanagement entity)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            // SQL sorgusu
            var sql = @"UPDATE processmanagement 
                    SET guid = @guid, 
                        hayvangirdi = @hayvangirdi, 
                        ilkkapikapandi = @ilkkapikapandi, 
                        kupeokundu = @kupeokundu, 
                        okunankupe = @okunankupe, 
                        sonagirlikalindimi = @sonagirlikalindimi, 
                        sonagirlik = @sonagirlik, 
                        cikiskapisiacildimi = @cikiskapisiacildimi, 
                        tarih = @tarih, 
                        cikisbeklemeagirligi = @cikisbeklemeagirligi, 
                        minimumhassasiyetagirlik = @minimumhassasiyetagirlik, 
                        cihazId = @cihazId, 
                        isTamamlandi = @isTamamlandi, 
                        mevcutMod = @mevcutMod 
                    WHERE id = @id";

            // Sorguyu yürütüyoruz
            var result = connection.Execute(sql, entity);
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
