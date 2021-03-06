﻿using System;
using Dapper;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Data
{
    public class PedidoRepository
    {


        public async Task<int>  InseriPedido(Pedido pedido)
        {
            int ret;
          
            using (var conexaoBD = new SqlConnection("Data Source=<ConnectioString>"))
            {
                try
                {
                    ret = await conexaoBD.ExecuteAsync(@"insert Pedido(IdPedido,Descricao)
                                    values (@IdPedido , @Descricao)", pedido);

                }
                catch (Exception ex)
                {

                    throw ex;
                }

              
            }
            return ret;
        }
            


    }
}
