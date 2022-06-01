using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebApplicationMVC.Controllers
{
    public class TransacoesController : Controller
    {
        private sdiasBdEntities db = new sdiasBdEntities();

        // GET: Transacoes
        public ActionResult Index()
        {
            var transacoes = db.Transacoes.Include(t => t.Conta).Include(t => t.TipoTransaco);
            return View(transacoes.ToList());
        }

        // GET: Transacoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaco transaco = db.Transacoes.Find(id);
            if (transaco == null)
            {
                return HttpNotFound();
            }
            return View(transaco);
        }

        // GET: Transacoes/Create
        public ActionResult Create()
        {
           
            ViewBag.ContaId = new SelectList(db.Contas, "Id", "Id");
            ViewBag.TipoTransacaoId = new SelectList(db.TipoTransacoes, "Id", "Descricao");
            return View();
        }

        // POST: Transacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ContaId,TipoTransacaoId,Valor,Data")] Transaco transaco)
        {
            Conta contaDB = db.Contas.Find(transaco.ContaId);

            double valor = transaco.Valor;
            double SaldoEmConta = contaDB.Saldo;
            double novoSaldo = 0;

            if (valor <= 0)
            {
                ModelState.AddModelError("Valor", "Não é permitido entrar com valor 0 ou negativo!");
                return View(transaco);
            }

            //deposito
            if (transaco.TipoTransacaoId == 1)
            {
                novoSaldo = SaldoEmConta + valor;
                contaDB.Saldo = novoSaldo;
                transaco.Valor = valor;
                transaco.Data = DateTime.Now;
            }

            //saque
            if (transaco.TipoTransacaoId == 2)
            {
                if (valor > SaldoEmConta)
                {
                    ModelState.AddModelError("Valor", "Erro! Valor solicitado é maior que o saldo na conta!");
                    ViewBag.ContaId = new SelectList(db.Contas, "Id", "Id", transaco.ContaId);
                    ViewBag.TipoTransacaoId = new SelectList(db.TipoTransacoes, "Id", "Descricao", transaco.TipoTransacaoId);
                    return View(transaco);
                }

                novoSaldo = SaldoEmConta - valor;
                contaDB.Saldo = novoSaldo;
                transaco.Valor = valor;
                transaco.Data = DateTime.Now;

            }
            if (ModelState.IsValid)
            {
                db.Transacoes.Add(transaco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContaId = new SelectList(db.Contas, "Id", "Id", transaco.ContaId);
            ViewBag.TipoTransacaoId = new SelectList(db.TipoTransacoes, "Id", "Descricao", transaco.TipoTransacaoId);
            return View(transaco);
        }

        // GET: Transacoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaco transaco = db.Transacoes.Find(id);
            if (transaco == null)
            {
                return HttpNotFound();
            }
            return View(transaco);
        }

        // POST: Transacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Transaco transaco = db.Transacoes.Find(id);
            db.Transacoes.Remove(transaco);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        //Maior Valor depositado
        public ActionResult MaiorValor()
        {
            try
            {
                double resultado = 0;
                var transacoes = db.Transacoes;
                if (transacoes.Any())
                {
                    resultado = db.Transacoes.Max(v => v.Valor);
                    ViewData["mensagem"] = "O maior valor depositado foi de: " + resultado + " reais.";
                }
                else
                {
                    ViewData["mensagem"] = "Não foi depositado nenhum valor ainda!";
                }

                //var listaMaior = db.Transacoes.OrderByDescending(item => item.Valor).First();
                return View();

            }
            catch (Exception ex)
            {

                ViewData["mensagem"] = "Nenhum valor foi depositado ainda!";
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
