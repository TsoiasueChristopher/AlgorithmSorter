namespace SortingVisualizer.Models
{
    public enum StepType
    {
        Compare,
        Swap,
        Sorted,
        Pivot

        
    }
    public record SortStep(int index1, int index2, StepType type);
    
}
