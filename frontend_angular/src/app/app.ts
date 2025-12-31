/**
 * @fileoverview Componente raíz de Seven Facturación UI.
 *
 * Define el shell principal de la aplicación con header
 * y router-outlet para navegación.
 *
 * @author Seven Facturación Team
 * @version 1.0.0
 */

import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {}
