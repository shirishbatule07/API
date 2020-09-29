
namespace Patheyam.Domain.Models
{
	using Patheyam.Common;
	using System;
	using System.Collections.Generic;
	public class ProductsListDomain
	{
		public List<ProductsDomain> Products { get; set; }
		public PaginationInfo Pagination { get; set; }
	}
	public class ProductsDomain : AuditDomain
	{
        public string ProductId { get; set; }
        public string CompanyId { get; set; }
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
    }
}
