-- --------------------------------------------------------
-- Servidor:                     127.0.0.1
-- Versão do servidor:           5.7.17-log - MySQL Community Server (GPL)
-- OS do Servidor:               Win64
-- HeidiSQL Versão:              9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Copiando estrutura do banco de dados para controle-estoque
CREATE DATABASE IF NOT EXISTS `controle-estoque` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `controle-estoque`;

-- Copiando estrutura para tabela controle-estoque.grupo_produto
CREATE TABLE IF NOT EXISTS `grupo_produto` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  `ativo` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela controle-estoque.grupo_produto: ~9 rows (aproximadamente)
/*!40000 ALTER TABLE `grupo_produto` DISABLE KEYS */;
INSERT INTO `grupo_produto` (`id`, `nome`, `ativo`) VALUES
	(2, 'verduras', 1),
	(3, 'cereais', 1),
	(4, 'carnes', 1),
	(5, 'transporte', 1),
	(6, 'segurança', 1),
	(7, 'frutas', 1),
	(8, 'calçados', 1),
	(9, 'ferramentas', 1),
	(10, 'doces', 1);
/*!40000 ALTER TABLE `grupo_produto` ENABLE KEYS */;

-- Copiando estrutura para tabela controle-estoque.perfil
CREATE TABLE IF NOT EXISTS `perfil` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  `ativo` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela controle-estoque.perfil: ~1 rows (aproximadamente)
/*!40000 ALTER TABLE `perfil` DISABLE KEYS */;
INSERT INTO `perfil` (`id`, `nome`, `ativo`) VALUES
	(2, 'gerente', 1);
/*!40000 ALTER TABLE `perfil` ENABLE KEYS */;

-- Copiando estrutura para tabela controle-estoque.produto
CREATE TABLE IF NOT EXISTS `produto` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela controle-estoque.produto: ~0 rows (aproximadamente)
/*!40000 ALTER TABLE `produto` DISABLE KEYS */;
/*!40000 ALTER TABLE `produto` ENABLE KEYS */;

-- Copiando estrutura para tabela controle-estoque.unidade_medida
CREATE TABLE IF NOT EXISTS `unidade_medida` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nome` varchar(50) NOT NULL,
  `sigla` varchar(3) NOT NULL,
  `ativo` tinyint(1) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela controle-estoque.unidade_medida: ~4 rows (aproximadamente)
/*!40000 ALTER TABLE `unidade_medida` DISABLE KEYS */;
INSERT INTO `unidade_medida` (`id`, `nome`, `sigla`, `ativo`) VALUES
	(2, 'Quilo', 'Kg', 1),
	(3, 'Peça', 'pc', 1),
	(4, 'Pacote', 'pt', 1),
	(5, 'Unidade', 'un', 1);
/*!40000 ALTER TABLE `unidade_medida` ENABLE KEYS */;

-- Copiando estrutura para tabela controle-estoque.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(50) NOT NULL,
  `senha` varchar(32) NOT NULL,
  `nome` varchar(100) NOT NULL,
  `id_perfil` int(4) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

-- Copiando dados para a tabela controle-estoque.usuario: ~1 rows (aproximadamente)
/*!40000 ALTER TABLE `usuario` DISABLE KEYS */;
INSERT INTO `usuario` (`id`, `login`, `senha`, `nome`, `id_perfil`) VALUES
	(2, 'gouveia', '202cb962ac59075b964b07152d234b70', 'José Alves', 2);
/*!40000 ALTER TABLE `usuario` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
