using System;
using System.Text;

namespace SysFrameworks
{
    public class Pagination
    {
        public static string SetPagination(long p_Curr_Page, long p_PageSize, long p_TotalRecords,String p_controller, string p_action,DataCollections p_criteria)
        {
            long v_TotalPage = (long) Math.Ceiling((decimal ) p_TotalRecords / p_PageSize);
            long range1_from, range1_to, range2_from, range2_to, range3_from, range3_to;
            StringBuilder stringBuilder = new StringBuilder();
            
            if (p_Curr_Page == 1)
            {
                stringBuilder.Append("< li class='paginate_button previous disabled'>");
                stringBuilder.Append(" <a href = '#'>Previous</a>");
                stringBuilder.Append("</li>");
            }
            else
            {
                stringBuilder.Append("< li class='paginate_button previous'>");
                stringBuilder.Append(" <a href = '/'" + p_controller + "/" + p_action + ">Previous</a>");
                stringBuilder.Append("</li>");
            }
            
            if (v_TotalPage - 10 >= 0) {
                if (p_Curr_Page >= v_TotalPage - 4 )
                {
					range1_from = 1; range1_to = 2;
			        range2_from = -1; range2_to = -2;
					range3_from = p_Curr_Page - 2; range3_to =v_TotalPage;

                }else if (p_Curr_Page <= 4){
			        range1_from = 1; range1_to = p_Curr_Page + 2;
					range2_from = -1; range2_to = -2;
					range3_from = v_TotalPage - 1;
                    range3_to = v_TotalPage;

                }else{
					range1_from = 1; range1_to = 2;
					range2_from = p_Curr_Page - 2; range2_to = p_Curr_Page + 2;
					range3_from = v_TotalPage - 1; range3_to = v_TotalPage;
                }
            }else{//list all
				range1_from = 1; range1_to  = v_TotalPage;
				range2_from = -1; range2_to = -2;
				range3_from = -1; range3_to = -2;
            }
            for (long i = range1_from; i <= range1_to; ++i){
                if (i != p_Curr_Page) {
                    stringBuilder.Append("< li class='paginate_button previous disabled'>");
                    stringBuilder.Append(i.ToString());
                    stringBuilder.Append("</li>");
                }
                else
                {
                    stringBuilder.Append("<li class='paginate_button active'>");
                    stringBuilder.Append(" <a href = '#'>" + i.ToString() +"</a>");
                    stringBuilder.Append("</li>");
                }  
            }
            if (range2_from < range2_to) {
                stringBuilder.Append("<li class='paginate_button active'>");
                stringBuilder.Append(" <a href = '#'>...</a>");
                stringBuilder.Append("</li>");
            }
            for (long i = range2_from; i <= range2_to; ++i)
            {
                if (i != p_Curr_Page)
                {
                    stringBuilder.Append("< li class='paginate_button previous disabled' >");
                    stringBuilder.Append(i.ToString());
                    stringBuilder.Append("</li>");
                }
                else
                {
                    stringBuilder.Append("<li class='paginate_button active'>");
                    stringBuilder.Append(" <a href = '#'>" + i.ToString() + "</a>");
                    stringBuilder.Append("</li>");
                }
            }
            for (long i = range3_from; i <= range3_to; ++i)
            {
                if (i != p_Curr_Page)
                {
                    stringBuilder.Append("< li class='paginate_button previous disabled' >");
                    stringBuilder.Append(i.ToString());
                    stringBuilder.Append("</li>");
                }
                else
                {
                    stringBuilder.Append("<li class='paginate_button active'>");
                    stringBuilder.Append(" <a href = '#'>" + i.ToString() + "</a>");
                    stringBuilder.Append("</li>");
                }
            }              
            return stringBuilder.ToString();
        }
    }
}
