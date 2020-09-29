
namespace Patheyam.Contract.Models
{
    using Patheyam.Common;
    using System;

    public class CompanyContract
	{
		public int Id { get; set; }
	
		public bool IsDeleted { get; set; }
		public long DeleterUserId { get; set; }
		public DateTime DeletionTime { get; set; }
		public string CompanyName { get; set; }
		public string contactNumber { get; set; }
		public string Email { get; set; }
		public string alternatenumber { get; set; }
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
