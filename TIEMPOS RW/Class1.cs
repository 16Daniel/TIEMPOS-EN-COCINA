﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Configuration;
namespace DataGridViewProgress
{
    public class DataGridViewProgressColumn : DataGridViewImageColumn
    {
        public DataGridViewProgressColumn()
        {
            CellTemplate = new DataGridViewProgressCell();
        }
    }
}
namespace DataGridViewProgress
{
    class DataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static DataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public DataGridViewProgressCell()
        {
            this.ValueType = typeof(int);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            int progressVal;
            if (value != null || (int)value < 0 )
                progressVal = (int)value;
            else
                progressVal = 1;


            float percentage = ((float)progressVal / Single.Parse(ConfigurationManager.AppSettings["MaxMin"]));  // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
            Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
            // Draws the cell grid
            base.Paint(g, clipBounds, cellBounds,
             rowIndex, cellState, value, formattedValue, errorText,
             cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));
            if (percentage >= 0.0)
            {
                // Draw the progress bar and the text
                if ((int)value < int.Parse(ConfigurationManager.AppSettings["Minutos"]))
                {
                    g.FillRectangle(new SolidBrush(Color.LimeGreen), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);


                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.Red), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);

                }
                g.DrawString(progressVal.ToString(), cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 2);
            }
         
            
        }
    }
}

