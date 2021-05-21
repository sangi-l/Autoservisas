using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace Autoservisas.Models
{
    public class Part
    {
        [DisplayName("ID")]
        public int PartID { get; set; }
        [DisplayName("Kaina")]
        [Required]
        public double Price { get; set; }

        [DisplayName("Pavadinimas")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Kodas")]
        [Required]
        public int Code { get; set; }

        [DisplayName("Žymos")]
        [Required]
        public string Tags { get; set; }

        [DisplayName("Nuotrauka")]
        [Required]
        public string Picture { get; set; }

        [DisplayName("Aprašymas")]
        [Required]
        public string Details { get; set; }

        [DisplayName("Likutis")]
        [Required]
        public int Ammount { get; set; }

        [DisplayName("Kokybė")]
        [Required]
        public int Quality { get; set; }

        [DisplayName("Kategorija")]
        [Required]
        public string Category { get; set; }

        [DisplayName("Originalumas")]
        [Required]
        public bool Originallity { get; set; }

        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["Database"].ToString();
            con = new SqlConnection(constring);
        }

        public List<Part> GetPart()
        {
            connection();
            List<Part> partlist = new List<Part>();

            SqlCommand cmd = new SqlCommand("GetPartDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                partlist.Add(
                    new Part
                    {
                        PartID = Convert.ToInt32(dr["id_detale"]),
                        Price = Convert.ToDouble(dr["kaina"]),
                        Name = Convert.ToString(dr["pavadinimas"]),
                        Code = Convert.ToInt32(dr["detales_kodas"]),
                        Tags = Convert.ToString(dr["zymos"]),
                        Picture = Convert.ToString(dr["nuotrauka"]),
                        Details = Convert.ToString(dr["aprasymas"]),
                        Ammount = Convert.ToInt32(dr["likutis"]),
                        Quality = Convert.ToInt32(dr["kokybe"]),
                        Category = Convert.ToString(dr["kategorija"]),
                        Originallity = Convert.ToBoolean(dr["orginalumas"])
                    });
            }
            return partlist;
        }

        public bool UpdateAmmount(Part model)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdatePartDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@id_detale", model.PartID);
            cmd.Parameters.AddWithValue("@likutis", model.Ammount);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool CheckIfDepleted()
        {
            List<int> Ammounts = new List<int>();
            connection();

            SqlCommand cmd = new SqlCommand("GetPartDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                int ammount = Convert.ToInt32(dr["likutis"]);
                if (ammount < 5)
                {
                    return true;
                }
            }

            return false;
        }

        public List<String> GetCategories()
        {
            List<String> Categories = new List<String>();
            connection();

            SqlCommand cmd = new SqlCommand("GetPartDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach(DataRow dr in dt.Rows)
            {
                string category = Convert.ToString(dr["kategorija"]);
                if (!Categories.Contains(category))
                {
                    Categories.Add(category);
                }
            }

            return Categories;
        }

        public List<Part> GetPartsFromCategory(string category)
        {
            List<Part> parts = new List<Part>();
            connection();

            SqlCommand cmd = new SqlCommand("GetPartDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                string cat = Convert.ToString(dr["kategorija"]);
                if (cat == category)
                {
                    parts.Add(
                    new Part
                    {
                        PartID = Convert.ToInt32(dr["id_detale"]),
                        Price = Convert.ToDouble(dr["kaina"]),
                        Name = Convert.ToString(dr["pavadinimas"]),
                        Code = Convert.ToInt32(dr["detales_kodas"]),
                        Tags = Convert.ToString(dr["zymos"]),
                        Picture = Convert.ToString(dr["nuotrauka"]),
                        Details = Convert.ToString(dr["aprasymas"]),
                        Ammount = Convert.ToInt32(dr["likutis"]),
                        Quality = Convert.ToInt32(dr["kokybe"]),
                        Category = cat,
                        Originallity = Convert.ToBoolean(dr["orginalumas"])
                    });
                }
            }
            return parts;
        }
    }
}