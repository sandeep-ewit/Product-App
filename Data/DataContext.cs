using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using ProductApp.Models;


namespace ProductApp.Data
{
    public class DataContext
    {
        private readonly string _sqlConnString;

        public DataContext(IConfiguration config)
        {
            this._sqlConnString = config.GetConnectionString("SqlConnString");
        }

        public List<ProductDataModel> GetProduct()
        {
            try
            {
                DataTable dt = new DataTable();
                List<ProductDataModel> _productList = new List<ProductDataModel>();

                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = _sqlConnString;
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("get_product_list", conn) { CommandType = CommandType.StoredProcedure };
                   
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                    conn.Close();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ProductDataModel _p = new ProductDataModel();
                            _p.Id = dr["id"].ToString();
                            _p.ProductName = dr["product_name"].ToString();
                            _p.ProductDescription = dr["product_desc"].ToString();
                            _p.Price = dr["product_price"].ToString();
                            _p.Discount = dr["product_discount"].ToString();

                            _productList.Add(_p);
                        }
                    }
                }
                return _productList;
            }
            catch
            {
                throw;
            }
        }

        public string UpdateProduct(ProductDataModel model)
        {
            try
            {
                DataTable dt = new DataTable();

                using SqlConnection conn = new SqlConnection();
                conn.ConnectionString = _sqlConnString;
                conn.Open();
                SqlCommand cmd = new SqlCommand("update_product", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@id", model.Id));
                cmd.Parameters.Add(new SqlParameter("@product_name", model.ProductName));
                cmd.Parameters.Add(new SqlParameter("@product_desc", model.ProductDescription));
                cmd.Parameters.Add(new SqlParameter("@product_price",model.Price));
                cmd.Parameters.Add(new SqlParameter("@product_discount", model.Discount));
                cmd.Parameters.Add(new SqlParameter("@user", "Sandeep"));

                using SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                conn.Close();

                return JsonConvert.SerializeObject(dt.Rows[0][0]);
            }
            catch
            {
                throw;
            }
        }

        public string DeleteProduct(ProductDataModel model)
        {
            try
            {
                DataTable dt = new DataTable();

                using SqlConnection conn = new SqlConnection();
                conn.ConnectionString = _sqlConnString;
                conn.Open();
                SqlCommand cmd = new SqlCommand("delete_product", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@id", model.Id));               

                using SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

                conn.Close();

                return JsonConvert.SerializeObject(dt.Rows[0][0]);
            }
            catch
            {
                throw;
            }
        }

    }
}
