using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS_Common.Enums
{
    public class FolderNamesEnum
    {
        public enum FolderTypes
        {
            Product = 1,
            Banner = 2,
            Category =3,
            BlogPicturesFiles = 4,
            BlogVideoFiles = 5,
            CompanyProfile = 6,
            CompanyTeaser = 7,
            CompanyPicture = 8
        }
        public static string GetFileName(FolderTypes fileNamesEnum)
        {
            switch (fileNamesEnum)
            {
                case FolderTypes.Product:
                    return "Product";
                case FolderTypes.Banner:
                    return "Banner";
                case FolderTypes.Category:
                    return "Category";
                case FolderTypes.BlogPicturesFiles:
                    return "BlogPicturesFiles";
                case FolderTypes.BlogVideoFiles:
                    return "BlogVideoFiles";
                case FolderTypes.CompanyProfile:
                    return "CompanyProfile";
                case FolderTypes.CompanyTeaser:
                    return "CompanyTeaser";
                case FolderTypes.CompanyPicture:
                    return "CompanyPicture";
                default:
                    return "";
            }
        }
    }
}
