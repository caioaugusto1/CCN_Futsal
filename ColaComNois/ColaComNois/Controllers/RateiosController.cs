﻿using AutoMapper;
using ColaComNois.Context.DB;
using ColaComNois.Entidades;
using ColaComNois.Filters;
using ColaComNois.Repository;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ColaComNois.Controllers
{
    [AutorizacaoFilter]
    [RoutePrefix("administrativo")]
    public class RateiosController : Controller
    {
        private DespesasRepository _despesasRepo;
        private JogadoresRepository _jogadoresRepo;
        private RateiosRepository _rateiosRepo;

        public RateiosController(DespesasRepository _despesasRepo, JogadoresRepository _jogadoresRepo, RateiosRepository _rateiosRepo)
        {
            this._despesasRepo = _despesasRepo;
            this._jogadoresRepo = _jogadoresRepo;
            this._rateiosRepo = _rateiosRepo;
        }

        [Route("listar-rateios")]
        public ActionResult Index()
        {
            var rateios = Mapper.Map<IList<Rateio>>(_rateiosRepo.ObterTodos());
            //IList<Rateio> rateios = Mapper.Map<IList<CCN_Rateios>, IList<Rateio>>(_rateiosRepo.ObterTodos());

            return View(rateios);
        }

        [Route("{id:int}/detalhe-jogador")]
        public ActionResult Details(int id)
        {
            var rateioPorId = _rateiosRepo.ObterPorId(id);
            var rateioViewModel = Mapper.Map<ccn_rateios, Rateio>(rateioPorId);

            ViewBag.Jogadores = _jogadoresRepo.ObterPorId(rateioPorId.IdJogador);
            ViewBag.Despesas = _despesasRepo.ObterPorId(rateioPorId.IdDespesa);
            ViewBag.Recebedor = _jogadoresRepo.ObterPorId(Convert.ToInt32(rateioPorId.IdRecebedor));

            return View(rateioViewModel);
        }

        [Route("cadastrar-rateio")]
        public ActionResult Create()
        {
            ViewBag.Jogadores = _jogadoresRepo.ObterAtivos();
            ViewBag.Despesas = _despesasRepo.ObterTodos();
            ViewBag.Recebedor = _jogadoresRepo.ObterComissao();

            return View();
        }

        [HttpPost]
        public ActionResult Create(Rateio rateio)
        {
            if (ModelState.IsValid)
            {
                var rateiosDomain = Mapper.Map<ccn_rateios>(rateio);
                _rateiosRepo.Adicionar(rateiosDomain);

                return RedirectToAction("Index");
            }
            else
            {
                return View(rateio);
            }
        }

        [Route("{id:int}/editar-rateio")]
        public ActionResult Edit(int id)
        {
            var despesaJogadorPorId = _rateiosRepo.ObterPorId(id);
            var despesaJogadoViewModel = Mapper.Map<ccn_rateios, Rateio>(despesaJogadorPorId);

            ViewBag.Jogadores = _jogadoresRepo.ObterTodos();
            ViewBag.Despesas = _despesasRepo.ObterTodos();
            ViewBag.Recebedor = _jogadoresRepo.ObterComissao();

            return View(despesaJogadoViewModel);
        }

        [HttpPost]
        public ActionResult Edit(Rateio rateio)
        {
            if (ModelState.IsValid)
            {
                var rateioDomain = Mapper.Map<Rateio, ccn_rateios>(rateio);
                _rateiosRepo.Editar(rateioDomain);

                return RedirectToAction("Index");
            }
            else
            {
                return View(rateio);
            }
        }

        [Route("{id:int}/deletar-rateio")]
        public ActionResult Delete(int id)
        {
            var rateioPorId = _rateiosRepo.ObterPorId(id);
            var rateioViewModel = Mapper.Map<ccn_rateios, Rateio>(rateioPorId);
            ViewBag.Jogadores = _jogadoresRepo.ObterPorId(rateioPorId.IdJogador);

            ViewBag.Despesas = _despesasRepo.ObterPorId(rateioPorId.IdDespesa);
            ViewBag.Recebedor = _jogadoresRepo.ObterPorId(Convert.ToInt32(rateioPorId.IdRecebedor));


            return View(rateioViewModel);
        }

        [HttpPost]
        public ActionResult Delete(Rateio rateio)
        {
            _rateiosRepo.Excluir(rateio.Id);
            return RedirectToAction("Index");
        }
    }
}
