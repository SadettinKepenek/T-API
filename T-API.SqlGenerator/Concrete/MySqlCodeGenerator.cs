using T_API.Entity.Concrete;
using T_API.SqlGenerator.Abstract;

namespace T_API.SqlGenerator.Concrete
{
    public class MySqlCodeGenerator:SqlCodeGenerator
    {
        public override string CreateTable(Table table)
        {
            throw new System.NotImplementedException();
        }

        public override string DropTable(Table table)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterTable(Table table)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateColumn(Column column)
        {
            throw new System.NotImplementedException();
        }

        public override string DropColumn(Column column)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterColumn(Column column)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateRelation(ForeignKey foreignKey)
        {
            throw new System.NotImplementedException();
        }

        public override string DropRelation(ForeignKey foreignKey)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterRelation(ForeignKey foreignKey)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateKey(Key key)
        {
            throw new System.NotImplementedException();
        }

        public override string DropKey(Key key)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterKey(Key key)
        {
            throw new System.NotImplementedException();
        }

        public override string CreateIndex(Index index)
        {
            throw new System.NotImplementedException();
        }

        public override string DropIndex(Index index)
        {
            throw new System.NotImplementedException();
        }

        public override string AlterIndex(Index index)
        {
            throw new System.NotImplementedException();
        }
    }
}