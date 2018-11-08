List<MulSql> DealSql = new List<MulSql>();//������sql����


//����ִ�е�sql
DealSql.Add(new MulSql
                    {
                        sql = string.Format("INSERT INTO Pay_JXC_Orders_{0}({1}) VALUES(@{2})", UF.TableNo, string.Join(",", TmpLs), string.Join(",@", TmpLs)),
                        paras = DbParas,
                        errinfo = new List<MulSqlCnd> { new MulSqlCnd { errmsg = "������Ϣд��ʧ��", errcnd = false, errcode = 1 } }
                    });



result.success = UF.ServerID.eDB().ExecuteDbTransaction(yuntrans =>
                {
    long errcode = -1;
    foreach (var d in DealSql)
    {
        errcode = UF.ServerID.eDB().ExecuteNonQuery(d.sql, d.paras, transaction: yuntrans);
        foreach (var e in d.errinfo)
        {
            if (e.errcnd)
            {
                if (errcode == e.errcode)
                {
                    result.msg = e.errmsg;
                    return false;
                }
            }
            else
            {
                if (errcode < e.errcode)
                {
                    result.msg = e.errmsg;
                    return false;
                }
            }
        }
    }
    return true;
});