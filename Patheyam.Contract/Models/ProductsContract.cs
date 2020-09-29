
namespace Patheyam.Contract.Models
{
    using Patheyam.Common;
    using System;

    public class ProductsContract
    {
        public int Id { get; set; }

        public int Companyid { get; set; }
        public string ProductName { get; set; }
        public string Techname { get; set; }
        public string Searchname { get; set; }
        public string Description { get; set; }
        public string PrimeImage { get; set; }
        public string Price { get; set; }
        public string SoldBy { get; set; }
        public string Specifications { get; set; }
        public string ProductURL { get; set; }
        public string AvgCustReview { get; set; }
        public string YrofLaunch { get; set; }
        public string StatementType { get; set; }

    }
}