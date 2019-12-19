using T_API.Entity.Concrete;

namespace T_API.SqlGenerator.Abstract
{
    public abstract class SqlCodeGenerator
    {
        public abstract string CreateTable(Table table);
        public abstract string DropTable(Table table);
        public abstract string AlterTable(Table table);

        public abstract string CreateColumn(Column column);
        public abstract string DropColumn(Column column);
        public abstract string AlterColumn(Column column);

        public abstract string CreateRelation(ForeignKey foreignKey);
        public abstract string DropRelation(ForeignKey foreignKey);
        public abstract string AlterRelation(ForeignKey foreignKey);

        public abstract string CreateKey(Key key);
        public abstract string DropKey(Key key);
        public abstract string AlterKey(Key key);

        public abstract string CreateIndex(Index index);
        public abstract string DropIndex(Index index);
        public abstract string AlterIndex(Index index);
    }
}