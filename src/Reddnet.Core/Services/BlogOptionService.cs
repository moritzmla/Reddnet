using BlogCoreEngine.Core.Entities;
using BlogCoreEngine.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCoreEngine.Core.Services
{
    public class BlogOptionService : IBlogOptionService
    {
        private readonly IAsyncRepository<OptionDataModel> optionRepository;

        public BlogOptionService(IAsyncRepository<OptionDataModel> optionRepository)
        {
            this.optionRepository = optionRepository;
        }

        public async Task<byte[]> GetLogo()
        {
            var options = await this.optionRepository.GetAll();
            return options.First().Logo;
        }

        public async Task<string> GetTitle()
        {
            var options = await this.optionRepository.GetAll();
            return options.First().Title;
        }

        public async Task SetLogo(byte[] logo)
        {
            var options = await this.optionRepository.GetAll();

            var defaultOption = options.First();
            defaultOption.Logo = logo;

            await this.optionRepository.Update(defaultOption);
        }

        public async Task SetTitle(string title)
        {
            var options = await this.optionRepository.GetAll();

            var defaultOption = options.First();
            defaultOption.Title = title;

            await this.optionRepository.Update(defaultOption);
        }
    }
}
