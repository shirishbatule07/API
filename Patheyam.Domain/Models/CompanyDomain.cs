
namespace Patheyam.Domain.Models
{
	using Patheyam.Common;
	using System;
	using System.Collections.Generic;
	public class CompanyListDomain
	{
		public List<CompanyDomain> Companies { get; set; }
		public PaginationInfo Pagination { get; set; }
	}
	public class CompanyDomain : AuditDomain
	{
		public int Id { get; set; }
		public DateTime CreationTime { get; set; }
		public long CreatorUserId { get; set; }
		public DateTime LastModificationTime { get; set; }
		public long LastModifierUserId { get; set; }
		public bool IsDeleted { get; set; }
		public long DeleterUserId { get; set; }
		public DateTime DeletionTime { get; set; }
		public string CompanyName { get; set; }
		public long contactNumber { get; set; }
		public string Email { get; set; }
		public long alternatenumber { get; set; }
		public string address { get; set; }
		public string GST { get; set; }
		public string city { get; set; }
		public string pincode { get; set; }
		public string CompanyLogo { get; set; }
		public string codetailinfo { get; set; }
		public string operationatimeworkingday { get; set; }
		public string yearofestablish { get; set; }
		public string modeofpayment { get; set; }
		public string wesiteurl { get; set; }
		public string googlemap { get; set; }
	}
}
