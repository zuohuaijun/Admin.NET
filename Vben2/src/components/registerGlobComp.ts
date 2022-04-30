import type { App } from 'vue';
import { Button } from './Button';
import { Input, Layout, Row, Col, Select, Checkbox, Switch, Table } from 'ant-design-vue';

export function registerGlobComp(app: App) {
  app
    .use(Input)
    .use(Button)
    .use(Layout)
    .use(Row)
    .use(Col)
    .use(Select)
    .use(Checkbox)
    .use(Switch)
    .use(Table);
}
