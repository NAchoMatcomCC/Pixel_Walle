public interface INodeVisitor
{
    void Visit(SpawnStmt node);
    void Visit(ColorCommand node);
    void Visit(SizeStmt node);
    void Visit(DrawLineStmt node);
    void Visit(DrawCircleStmt node);
    void Visit(DrawRectangleStmt node);
    void Visit(FillStmt node);
    void Visit(AssignmentStmt node);
    void Visit(Label node);
    void Visit(GoTo node);

    
    // Agrega más nodos según sea necesario
}