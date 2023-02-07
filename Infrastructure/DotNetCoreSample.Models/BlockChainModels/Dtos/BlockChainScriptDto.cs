namespace  AuthService.Common.Models.BlockChainModels.Dtos
{
    public class BlockChainScriptDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Script { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }

}
