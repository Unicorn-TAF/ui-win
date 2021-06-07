using System;
using System.Collections.Generic;
using UIAutomationClient;

namespace Unicorn.UI.Win.Controls
{
    /// <summary>
    /// Extensions for automation element.
    /// </summary>
    public static class AutomationElementExtension
    {
        private static readonly Dictionary<Type, int> patterns = new Dictionary<Type, int>
        {
            { typeof(IUIAutomationInvokePattern), UIA_PatternIds.UIA_InvokePatternId },
            { typeof(IUIAutomationItemContainerPattern), UIA_PatternIds.UIA_ItemContainerPatternId },
            { typeof(IUIAutomationVirtualizedItemPattern), UIA_PatternIds.UIA_VirtualizedItemPatternId },
            { typeof(IUIAutomationSynchronizedInputPattern), UIA_PatternIds.UIA_SynchronizedInputPatternId },
            { typeof(IUIAutomationObjectModelPattern), UIA_PatternIds.UIA_ObjectModelPatternId },
            { typeof(IUIAutomationAnnotationPattern), UIA_PatternIds.UIA_AnnotationPatternId },
            { typeof(IUIAutomationTextPattern2), UIA_PatternIds.UIA_TextPattern2Id },
            { typeof(IUIAutomationLegacyIAccessiblePattern), UIA_PatternIds.UIA_LegacyIAccessiblePatternId },
            { typeof(IUIAutomationStylesPattern), UIA_PatternIds.UIA_StylesPatternId },
            { typeof(IUIAutomationSpreadsheetItemPattern), UIA_PatternIds.UIA_SpreadsheetItemPatternId },
            { typeof(IUIAutomationTransformPattern2), UIA_PatternIds.UIA_TransformPattern2Id },
            { typeof(IUIAutomationTextChildPattern), UIA_PatternIds.UIA_TextChildPatternId },
            { typeof(IUIAutomationDragPattern), UIA_PatternIds.UIA_DragPatternId },
            { typeof(IUIAutomationDropTargetPattern), UIA_PatternIds.UIA_DropTargetPatternId },
            { typeof(IUIAutomationTextEditPattern), UIA_PatternIds.UIA_TextEditPatternId },
            { typeof(IUIAutomationSpreadsheetPattern), UIA_PatternIds.UIA_SpreadsheetPatternId },
            { typeof(IUIAutomationCustomNavigationPattern), UIA_PatternIds.UIA_CustomNavigationPatternId },
            { typeof(IUIAutomationScrollItemPattern), UIA_PatternIds.UIA_ScrollItemPatternId },
            { typeof(IUIAutomationTogglePattern), UIA_PatternIds.UIA_TogglePatternId },
            { typeof(IUIAutomationSelectionPattern), UIA_PatternIds.UIA_SelectionPatternId },
            { typeof(IUIAutomationValuePattern), UIA_PatternIds.UIA_ValuePatternId },
            { typeof(IUIAutomationRangeValuePattern), UIA_PatternIds.UIA_RangeValuePatternId },
            { typeof(IUIAutomationScrollPattern), UIA_PatternIds.UIA_ScrollPatternId },
            { typeof(IUIAutomationExpandCollapsePattern), UIA_PatternIds.UIA_ExpandCollapsePatternId },
            { typeof(IUIAutomationGridPattern), UIA_PatternIds.UIA_GridPatternId },
            { typeof(IUIAutomationTransformPattern), UIA_PatternIds.UIA_TransformPatternId },
            { typeof(IUIAutomationGridItemPattern), UIA_PatternIds.UIA_GridItemPatternId },
            { typeof(IUIAutomationWindowPattern), UIA_PatternIds.UIA_WindowPatternId },
            { typeof(IUIAutomationSelectionItemPattern), UIA_PatternIds.UIA_SelectionItemPatternId },
            { typeof(IUIAutomationDockPattern), UIA_PatternIds.UIA_DockPatternId },
            { typeof(IUIAutomationTablePattern), UIA_PatternIds.UIA_TablePatternId },
            { typeof(IUIAutomationTableItemPattern), UIA_PatternIds.UIA_TableItemPatternId },
            { typeof(IUIAutomationTextPattern), UIA_PatternIds.UIA_TextPatternId },
            { typeof(IUIAutomationMultipleViewPattern), UIA_PatternIds.UIA_MultipleViewPatternId },
            { typeof(IUIAutomationSelectionPattern2), UIA_PatternIds.UIA_SelectionPattern2Id },
        };

        /// <summary>
        /// Get pattern of the specified type from the control.
        /// </summary>
        /// <typeparam name="T">pattern type</typeparam>
        /// <param name="element"></param>
        /// <returns>pattern instance</returns>
        public static T GetPattern<T>(this IUIAutomationElement element) where T : class
        {
            var type = typeof(T);
            if (patterns.ContainsKey(type))
            {
                return element.GetCurrentPattern(patterns[type]) as T;
            }
            else
            {
                throw new ArgumentException($"UIA pattern '{type}' does not exist");
            }
        }
    }
}
