/**
 * @fileoverview Servicio HTTP base abstracto para operaciones CRUD.
 * 
 * Proporciona una implementación genérica del patrón Repository para
 * comunicación con la API REST, eliminando código duplicado en servicios
 * específicos de dominio.
 * 
 * @architectural_layer Core/Services
 * @pattern Abstract Factory, Template Method, Repository
 * @author Seven Facturación Team
 * @version 1.0.0
 * 
 * @generic_types
 * - TEntity: Tipo de entidad del dominio
 * - TCreateDto: DTO para creación de entidades
 * - TUpdateDto: DTO para actualización de entidades
 * 
 * @usage
 * Los servicios de dominio extienden esta clase base:
 * @example
 * export class ClienteService extends BaseHttpService<Cliente, CrearClienteDto, ActualizarClienteDto> {
 *   constructor(http: HttpClient) {
 *     super(http, API_CONFIG.endpoints.clientes);
 *   }
 * }
 * 
 * @benefits
 * - DRY: Elimina duplicación de código CRUD
 * - Consistencia: Todas las operaciones siguen el mismo patrón
 * - Testabilidad: Fácil de mockear en tests unitarios
 * - Extensibilidad: Permite override de métodos específicos
 */

import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_CONFIG } from '../config/api.config';

/**
 * Servicio HTTP base abstracto para operaciones CRUD genéricas.
 * 
 * @abstract
 * @template TEntity - Tipo de entidad de dominio (response)
 * @template TCreateDto - DTO para crear entidades (request)
 * @template TUpdateDto - DTO para actualizar entidades (request)
 */
export abstract class BaseHttpService<TEntity, TCreateDto, TUpdateDto> {
  /** URL base completa del endpoint */
  protected readonly apiUrl: string;

  /**
   * Constructor del servicio base.
   * 
   * @param http - Cliente HTTP de Angular
   * @param endpoint - Endpoint relativo (ej: '/Clientes')
   */
  constructor(
    protected readonly http: HttpClient,
    protected readonly endpoint: string
  ) {
    this.apiUrl = `${API_CONFIG.baseUrl}${endpoint}`;
  }

  /**
   * Obtiene todas las entidades del recurso.
   * 
   * @returns Observable con array de entidades
   * 
   * @httpMethod GET
   * @endpoint /{resource}
   */
  getAll(): Observable<TEntity[]> {
    return this.http.get<TEntity[]>(this.apiUrl);
  }

  /**
   * Obtiene una entidad por su ID.
   * 
   * @param id - Identificador único de la entidad
   * @returns Observable con la entidad encontrada
   * 
   * @httpMethod GET
   * @endpoint /{resource}/{id}
   */
  getById(id: number): Observable<TEntity> {
    return this.http.get<TEntity>(`${this.apiUrl}/${id}`);
  }

  /**
   * Crea una nueva entidad.
   * 
   * @param dto - DTO con datos de creación
   * @returns Observable con la entidad creada
   * 
   * @httpMethod POST
   * @endpoint /{resource}
   */
  create(dto: TCreateDto): Observable<TEntity> {
    return this.http.post<TEntity>(this.apiUrl, dto);
  }

  /**
   * Actualiza una entidad existente.
   * 
   * @param id - Identificador de la entidad
   * @param dto - DTO con datos actualizados
   * @returns Observable con la entidad actualizada
   * 
   * @httpMethod PUT
   * @endpoint /{resource}/{id}
   */
  update(id: number, dto: TUpdateDto): Observable<TEntity> {
    return this.http.put<TEntity>(`${this.apiUrl}/${id}`, dto);
  }

  /**
   * Elimina una entidad.
   * 
   * @param id - Identificador de la entidad
   * @returns Observable void (204 No Content)
   * 
   * @httpMethod DELETE
   * @endpoint /{resource}/{id}
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  /**
   * Helper para construir HttpParams desde un objeto.
   * 
   * @param params - Objeto con parámetros query
   * @returns HttpParams configurados
   * 
   * @protected
   */
  protected buildParams(params: Record<string, string | number | boolean | undefined>): HttpParams {
    let httpParams = new HttpParams();
    Object.entries(params).forEach(([key, value]) => {
      if (value !== undefined && value !== null) {
        httpParams = httpParams.set(key, String(value));
      }
    });
    return httpParams;
  }
}

