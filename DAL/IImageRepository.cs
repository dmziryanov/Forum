using System;
using System.Data;
using System.Data.Linq;

namespace DAL
{
    public interface IImageRepository
    {
        Byte[] Get(int id);

        ImageFile GetFileName(int id);
        void ReleaseContext();
        int Save(ImageFile imageFile);
    }
}