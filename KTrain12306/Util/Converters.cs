using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace KTrain12306.Util
{
    public class ListViewWidthToItemWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double listViewActualWidth && parameter is string columnsString)
            {
                if (int.TryParse(columnsString, out int desiredColumns))
                {
                    if (desiredColumns > 0)
                    {
                        // 考虑 ListView 自身的左右 Margin，这里假设是 10 + 10 = 20
                        // 确保这个值与你的 ListView Margin="10,10,10,0" 匹配
                        double listViewHorizontalMargin = 20; // 你的 ListView 左右各有 10 像素的 Margin
                        double availableWidthForItems = listViewActualWidth - listViewHorizontalMargin;

                        // 每个 Item 之间的间距 (可选，如果 ItemsWrapGrid 默认没有间距，或者间距很小)
                        // UWP 的 ItemsWrapGrid 默认 ItemWidth 和 ItemHeight 之间没有额外的空间。
                        // 如果你需要 Item 之间有间距，你需要：
                        // 1. 在 ItemTemplate 根 Grid 上加 Margin
                        // 2. 在这里计算时减去这些间距的总和
                        // 为了简化，我们假设 ItemTemplate 根 Grid 没有横向 Margin，让 ItemsWrapGrid 自动填充。

                        double calculatedItemWidth = availableWidthForItems / desiredColumns;

                        // 假设你的每个列车项目内容的最小合理宽度是 280-300 像素
                        // 如果计算出来的 ItemWidth 小于这个值，可能会导致内容被挤压
                        // 你可以设置一个最小限制，但 ItemsWrapGrid 会自动换行
                        // 所以这里主要保证平分
                        double minComfortableItemWidth = 310; // 根据你的内容调整这个值

                        // 确保计算出来的宽度不小于最小舒适宽度，但这可能会导致列数不是期望的
                        // 更推荐的做法是让 ItemsWrapGrid 自动计算，但是由于不能用 MinimumItemWidth，
                        // 我们只能强制计算。
                        // 这里我们就是简单地平分，如果太小，用户会看到挤压。
                        // 或者你可以让它不小于某个值，但这样就不是精确的 n 列了
                        return Math.Max(minComfortableItemWidth, calculatedItemWidth);
                    }
                }
            }
            return 0.0; // 返回默认值
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
