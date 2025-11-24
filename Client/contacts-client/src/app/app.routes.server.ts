import { RenderMode, ServerRoute } from '@angular/ssr';

// Use server-side rendering (no prerendering) for dynamic routes to avoid
// requiring `getPrerenderParams` for parametric routes during build.
export const serverRoutes: ServerRoute[] = [
  {
    path: '**',
    renderMode: RenderMode.Server
  }
];
