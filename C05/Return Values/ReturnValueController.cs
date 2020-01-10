using Microsoft.AspNetCore.Mvc;

namespace Return_Values
{
    [Route("/")]
    public class ReturnValueController : ControllerBase
    {
        [HttpGet("ActionResultMyResult/{input}")]
        public ActionResult<MyResult> GetActionResultMyResult(int input)
        {
            return Ok(new MyResult
            {
                Input = input,
                Value = "GetActionResultMyResult"
            });
        }

        [HttpGet("ActionResultMyResult/{input}")]
        public ActionResult<MyResult> GetMyResult(int input)
        {
            return new MyResult
            {
                Input = input,
                Value = "GetActionResultMyResult"
            };
        }

        [HttpGet("ActionResultMyResult/{input}")]
        public IActionResult GetIActionResult(int input)
        {
            return Ok(new MyResult
            {
                Input = input,
                Value = "GetActionResultMyResult"
            });
        }
    }
}
