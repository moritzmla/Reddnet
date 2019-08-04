using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlogCoreEngine.Core.Interfaces
{
    public interface IBlogOptionService
    {
        Task SetTitle(string title);

        Task SetLogo(byte[] logo);

        Task<string> GetTitle();

        Task<byte[]> GetLogo();
    }
}
