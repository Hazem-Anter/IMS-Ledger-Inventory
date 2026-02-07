
namespace IMS.Application.Features.Reports.Queries.StockValuation
{
    // This enum defines the different modes of stock valuation that can be used in the stock valuation report.
    // The WeightedAverage mode calculates the average cost of inventory based on the total cost and total quantity of items in stock.
    // The Fifo (First In, First Out) mode assumes that the oldest inventory items are sold first, and the cost of goods sold is based on the cost of the oldest inventory.
    // These modes can be used to determine how the value of inventory is calculated in the stock valuation report, which can impact financial reporting and decision-making.
    public enum StockValuationMode
    {
        WeightedAverage = 1,
        Fifo = 2
    }
}
