
using CZManageSystem.Data.Domain.Composite;
using CZManageSystem.Data.Domain.SysManger;
using System.IO;
using ZManageSystem.Core;

namespace CZManageSystem.Service.Composite
{
    public interface IVoteQuestionTempService : IBaseService<VoteQuestionTemp>
    {
        dynamic  ImportQuestion(Stream fileStream, Users user);
    }
}