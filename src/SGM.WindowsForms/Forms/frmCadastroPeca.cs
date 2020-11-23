﻿using SGM.ApplicationServices.Application.Interface;
using SGM.Domain.Entities;
using System;
using System.Windows.Forms;

namespace SGM.WindowsForms
{
    public partial class FrmCadastroPeca : FrmModeloDeFormularioDeCadastro
    {
        private readonly IPecaApplication _pecaApplication;

        public FrmCadastroPeca(IPecaApplication pecaApplication)
        {
            _pecaApplication = pecaApplication;
            InitializeComponent();
        }

        public void LimpaTela()
        {
            txtPecaId.Clear();
            txtPeca.Clear();
            txtFornecedor.Clear();
            txtValorPeca.Clear();
            txtValorFrete.Clear();
        }

        private void BtnInserir_Click(object sender, EventArgs e)
        {
            this.operacao = "inserir";
            this.AlteraBotoes(2);
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            this.operacao = "alterar";
            this.AlteraBotoes(2);
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult d = MessageBox.Show("Deseja realmente excluir o registro?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (d.ToString() == "Yes")
                {
                    _pecaApplication.InativarPeca(Convert.ToInt32(txtPecaId.Text));

                    MessageBox.Show("Registro Excluído com Sucesso!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.LimpaTela();
                    this.AlteraBotoes(1);
                }
            }
            catch
            {
                MessageBox.Show("Impossível excluir o registro. \n O registro está sendo utilizado em outro local.", "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.AlteraBotoes(3);
            }
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Peca peca = new Peca
                {
                    Descricao = txtPeca.Text,
                    Fornecedor = txtFornecedor.Text,
                    Valor = Convert.ToDecimal(txtValorPeca.Text.Replace("R$ ", "")),
                    ValorFrete = Convert.ToDecimal(txtValorFrete.Text.Replace("R$ ", "")),
                    Ativo = true,
                    DataCadastro = DateTime.Now
                };

                if (this.operacao == "inserir")
                {
                    _pecaApplication.SalvarPeca(peca);
                    MessageBox.Show("Cadastro inserido com sucesso! Peça/Produto: " + peca.Descricao.ToString(), "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    peca.PecaId = Convert.ToInt32(txtPecaId.Text);
                    _pecaApplication.AtualizarPeca(peca);

                    MessageBox.Show("Cadastro alterado com sucesso! Peça/Produto: " + peca.Descricao.ToString());
                }

                this.LimpaTela();
                this.AlteraBotoes(2);
            }
            catch (Exception erro)
            {
                MessageBox.Show(erro.Message, "ERRO!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.operacao = "cancelar";
            this.AlteraBotoes(1);
            this.LimpaTela();
        }

        private void BtnLocalizar_Click(object sender, EventArgs e)
        {

            frmConsultaPeca formConsultaPeca = new frmConsultaPeca();
            formConsultaPeca.ShowDialog();
            if (formConsultaPeca.codigo != 0)
            {
                var peca = _pecaApplication.GetPecaByPecaId(formConsultaPeca.codigo);

                txtPecaId.Text = Convert.ToString(peca.PecaId);
                txtPeca.Text = Convert.ToString(peca.Descricao);
                txtFornecedor.Text = Convert.ToString(peca.Fornecedor);
                txtValorPeca.Text = Convert.ToString(peca.Valor);
                txtValorFrete.Text = Convert.ToString(peca.ValorFrete);

                AlteraBotoes(3);
            }
            else
            {
                this.LimpaTela();
                this.AlteraBotoes(1);
            }

            formConsultaPeca.Dispose(); //destrói o formulário de consulta, para não ocupar memória.
        }

        private void TxtValorPeca_Leave(object sender, EventArgs e)
        {
            try
            {
                Decimal VP = Convert.ToDecimal(txtValorPeca.Text.Replace("R$ ", "0"));
                txtValorPeca.Text = Convert.ToString(VP.ToString("C"));
            }
            catch (Exception validaVP)
            {

                MessageBox.Show("Por favor, digite um número. \n " + validaVP.Message, "ALERTA!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtValorPeca.Clear();
                txtValorPeca.Focus();
            }

        }

        private void TxtValorFrete_Leave(object sender, EventArgs e)
        {
            try
            {
                Decimal VF = Convert.ToDecimal(txtValorFrete.Text.Replace("R$ ", ""));
                txtValorFrete.Text = Convert.ToString(VF.ToString("C"));
            }
            catch (Exception validaVF)
            {

                MessageBox.Show("Por favor, digite um número. \n " + validaVF.Message, "ALERTA!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtValorFrete.Clear();
                txtValorFrete.Focus();
            }
        }
    }
}