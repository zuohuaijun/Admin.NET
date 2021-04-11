using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dilon.Core.Service
{
    public interface ICodeGenConfigService
    {
        Task Add(CodeGenConfig input);
        void AddList(List<TableColumnOuput> tableColumnOuputList, SysCodeGen codeGenerate);
        string ConvertDataType(string dataType);
        Task Delete(long codeGenId);
        Task<SysCodeGenConfig> Detail(CodeGenConfig input);
        Task<List<CodeGenConfig>> List([FromQuery] CodeGenConfig input);
        Task Update(List<CodeGenConfig> inputList);
    }
}