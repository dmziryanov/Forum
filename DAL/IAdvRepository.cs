using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DAL
{
    public class RegisterModel
    {
        [Key]
        public int UserId { get; set; }

        public int UserType { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "About")]
        public string About { get; set; }

        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool HideNumber { get; set; }

        [Required]
        public string Phone { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }

        public DateTime Created { get; set; }
        public string Address { get; set; }
        public int? UserAvatarId { get; set; }
    }

    public interface IAdvRepository
    {
        List<UserProfile> GetAllUsers();
        IEnumerable<CarAdv> GetAll();

        IEnumerable<AdvModel> GetFilteredSortedPageResult(AdvType type, AdvCondition condition, string keywords, string country, int location, int pageSize, int currentPage, string SortBy, double minPrice, double maxPrice);
        IEnumerable<AdvModel> GetFilteredSortedPageResult(AdvType type, AdvCondition condition, List<int> category, string country, int location, int pageSize, int currentPage, string SortBy, double minPrice, double maxPrice);
        IEnumerable<AdvModel> GetFilteredSortedPageResult(AdvType type, AdvCondition condition, int category, string keywords, string country, int location, int pageSize, int currentPage, string OrderExpr, double minPrice, double maxPrice);

        AdvModel Get(int id);

        IEnumerable<AdvModel> Get(int[] id);

        IEnumerable<AdvModel> GetSimilar(AdvModel sample);

        CarAdvModel GetCar(int id);
        void Save(AdvModel adv);
        void AddMessage(SiteMessage model);
        UserProfile GetUser(int currentUserId);
        void SaveUser(UserProfile model);
        List<AdvModel> GetByUserId(int currentUserId);
        void Delete(int id);
        void IncreaseViewCount(int toInt32, int viewCount);
        void SaveSearch(int currentUserId, string keywords, string location);


        void SaveCar(CarAdvModel model);
        void SaveBlogImage(int id, int photoId);
        int[] GetPhotoIds(int id);
        int[] GetFileIds(int id);

        string[] GetFileNames(int id);
        void SaveBlogFile(int id, int photoId);
        int GetAdvCountByUserId(int userId);
        int GetFavoriteCountByUserId(int userId);
        int GetViewCountByUserId(int userId);
        void UpdateLoginDate(string userId);

        DateTime GetLoginDate(string userId);
        void CloseAccount(int currentUserId, int inlineRadioOptions);
        HashSet<int> GetFavorite(string serverVariable);
        void AddFavorite(string serverVariable, int i);
        void Remove(string serverVariable, int id);
        HashSet<int> GetChosenAdded(string serverVariable);
        HashSet<int> CheckChosenAdded(string serverVariable);
    }

    [System.ComponentModel.DataAnnotations.Schema.Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Display(Name = "About")]
        public string About { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        public string Password { get; set; }

        [Display(Name = "User name")]
        public int? Gender { get; set; }


        public bool HideNumber { get; set; }

        
        public string Phone { get; set; }

        
        public string LastName { get; set; }
        

        public string FirstName { get; set; }

        public string WorkGoal { get; set; }

        public int? UserType { get; set; }
        public int? UserAvatarId { get; set; }

        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [NotMapped]
        public string OldPassword { get; set; }

        [NotMapped]
        public string NewPassword { get; set; }

        [NotMapped]
        public string ConfirmPassword { get; set; }
    }

   
}